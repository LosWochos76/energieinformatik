#!/usr/bin/python3
# import the list of renewable powerplants in germany into a postgres database

from datetime import date
import os
import pandas as pd
from sqlalchemy import create_engine
from io import BytesIO
from urllib.request import urlopen
import zipfile
import urllib
import ssl

engine = create_engine('postgresql://admin:secret@localhost:5432/postgres')

try:
	print("Lösche alte Daten in der Datenbank...")
	with engine.connect() as con:
		con.execute('drop table energieinformatik.renewable_power_plants;')
except:
	print("Einige Tabellen konnten nicht gelöscht werden!");

zipurl = 'https://data.open-power-system-data.org/renewable_power_plants/2020-08-25/renewable_power_plants_DE.csv'

#try:
#	print("Lade CSV-Datei herunter...")
#	ctx = ssl.create_default_context()
#	ctx.check_hostname = False
#	ctx.verify_mode = ssl.CERT_NONE
#	with urllib.request.urlopen(zipurl, context=ctx) as u, open("renewable_power_plants_DE.csv", 'wb') as f:
#	    f.write(u.read())
#except:
#	printf("Datei konnte nicht heruntergeladen werden!");

#try:
print("Lade Daten...")
df = pd.read_csv('renewable_power_plants_DE.csv', sep=',')
#df['commissioning_date'] = pd.to_datetime(df['commissioning_date'])
#df['decommissioning_date'] = pd.to_datetime(df['decommissioning_date'])

print("Schreibe Daten in Datenbank...")
df.to_sql('renewable_power_plants', engine, index=False, if_exists='append', chunksize=100)
#except:
#	print("Konnte die Daten nicht in die Datenbank importieren")

#try:
#	print("Lösche CSV-Dateien...")
#	os.remove("renewable_power_plants_DE.csv")
#except:
#	print("Einige heruntergeladene Dateien konnten nicht gelöscht werden!")