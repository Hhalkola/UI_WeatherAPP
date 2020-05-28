from time import sleep
from sense_hat import SenseHat
import MySQLdb
import mysql.connector
import os

def get_cpu_temp():
  res = os.popen("vcgencmd measure_temp").readline()
  t = float(res.replace("temp=","").replace("'C\n",""))
  return(t)

sense = SenseHat()
server = ''
database = ''
username = ''
password = ''

while True:
    try:
        cnx =mysql.connector.connect(user=username, password=password, host=server,database=database)
        cursor = cnx.cursor()
        prec = 1

        p =(f"{sense.get_pressure():.{prec}f}")
        h =(f"{sense.get_humidity():.{prec}f}")

        t = sense.get_temperature_from_humidity()
        t_cpu = get_cpu_temp()
        
        
        # calculates the real temperature compesating CPU heating
        t_corr = t - ((t_cpu-t)/1.5)
        t_corr = (f"{t_corr:.{prec}f}")
        
        s = """insert into weatherdb.weather(datevalue,temperature, humidity, pressure) VALUES (now(),%s,%s,%s)"""
        print(s)
        cursor.execute(s,(t_corr,h,p))
        cnx.commit()
        cursor.close()
        cnx.close()
        print('Inserted into DB', t_corr, h , p )
        sleep(300)
    except:
        print('elp')
