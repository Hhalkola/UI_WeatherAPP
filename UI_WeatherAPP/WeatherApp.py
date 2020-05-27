from time import sleep
import mysql.connector
from sense_hat import SenseHat

sleep(60)

sense = SenseHat()
server = ''
database = ''
username = ''
password = ''
port = ''

while True:
    cnx =mysql.connector.connect(user=username, password=password, host=server,database=database)
    cursor = cnx.cursor()
    prec = 1

    t =(f"{sense.get_temperature():.{prec}f}")
    p =(f"{sense.get_pressure():.{prec}f}")
    h =(f"{sense.get_humidity():.{prec}f}")

    s = """insert into <table>(datevalue,temperature, humidity, pressure) VALUES (now(),%s,%s,%s)"""
    print(s)
    cursor.execute(s,(t,h,p))
    cnx.commit()
    cursor.close()
    cnx.close()
    print('Inserted into DB')
    
    sleep(3000)
