#!/usr/bin/python3
# Import weather-data from uni bayreuth into a postgres-database

from datetime import date
import os
import pandas as pd
from sqlalchemy import create_engine
from io import BytesIO
from urllib.request import urlopen
import zipfile
import urllib
import ssl

def makeInt(df, name):
	df[name] = df[name].astype(int)

#engine = create_engine('postgresql://hshl:hshl@localhost:5432/hshl')
engine = create_engine('postgresql://admin:secret@localhost:5432/postgres')

try:
	with engine.connect() as con:
		con.execute('drop table wetter.wetterstation;')
		con.execute('drop table wetter.wettermessung;')
except:
	print("Konnte alte Tabellen nicht löschen!")

today = date.today()
d = today.strftime("%Y-%m-%d")
zipurl = 'https://dbup2date.uni-bayreuth.de/downloads/wetterdaten/{}_wetterdaten_CSV.zip'.format(d)

try:
	print("Lade Zip-Datei herunter...")
	ctx = ssl.create_default_context()
	ctx.check_hostname = False
	ctx.verify_mode = ssl.CERT_NONE
	with urllib.request.urlopen(zipurl, context=ctx) as u, open("wetterdaten.zip", 'wb') as f:
	    f.write(u.read())
except:
	print(zipurl)
	print("Datei konnte nicht heruntergeladen werden!");

try:
	print("Entpacke Zip-Archiv...")
	with zipfile.ZipFile("wetterdaten.zip", 'r') as zip_ref:
	    zip_ref.extractall(".")
except:
	print("Datei konnte nicht entpackt werden!")

try:
	print("Importiere Wetterstationen...")
	cols = ['S_ID', 'Standort', 'Geo_Breite', 'Geo_Laenge', 'Hoehe', 'Betreiber']
	df = pd.read_csv("wetterdaten_Wetterstation.csv", sep=";", encoding = "ISO-8859-1", decimal=",", usecols=cols)
	df.columns = ['s_id','standort','geo_breite','geo_laenge','hoehe','betreiber']
	df['s_id'] = df['s_id'].astype(int)
	df['geo_breite'] = df['geo_breite'].astype(float)
	df['geo_laenge'] = df['geo_laenge'].astype(float)
	df['hoehe'] = df['hoehe'].astype(float)
	df.to_sql('wetterstation', engine, index=False, if_exists='replace', schema='wetter')
except:
	print("Import der Wetterstationen fehlgeschlagen!")

try:
	print("Importiere Wettermessungen...")
	df = pd.read_csv("wetterdaten_Wettermessung.csv", sep=";",
		encoding = "ISO-8859-1", decimal=",", 
		usecols={"Stations_ID", "Datum", "Qualitaet", "Min_5cm", "Min_2m", "Mittel_2m",
		"Max_2m","Relative_Feuchte","Mittel_Windstaerke","Max_Windgeschwindigkeit",
		"Sonnenscheindauer","Mittel_Bedeckungsgrad","Niederschlagshoehe","Mittel_Luftdruck"})
	df.columns = ['stations_id','datum','qualitaet','min_5cm','min_2m','mittel_2m',
		"max_2m","relative_feuchte","mittel_windstaerke","max_windgeschwindigkeit",
		"sonnenscheindauer","mittel_bedeckungsgrad","niederschlagshoehe","mittel_luftdruck"]
	df['stations_id'] = df['stations_id'].astype(int)
	df['datum'] = pd.to_datetime(df['datum'])
	df['qualitaet'] = df['qualitaet'].astype(pd.Int32Dtype())
	df['min_5cm'] = df['min_5cm'].astype(float)
	df['min_2m'] = df['min_2m'].astype(float)
	df['mittel_2m'] = df['mittel_2m'].astype(float)
	df['max_2m'] = df['max_2m'].astype(float)
	df['relative_feuchte'] = df['relative_feuchte'].astype(float)
	df['mittel_windstaerke'] = df['mittel_windstaerke'].astype(float)
	df['max_windgeschwindigkeit'] = df['max_windgeschwindigkeit'].astype(float)
	df['sonnenscheindauer'] = df['sonnenscheindauer'].astype(float)
	df['mittel_bedeckungsgrad'] = df['mittel_bedeckungsgrad'].astype(float)
	df['niederschlagshoehe'] = df['niederschlagshoehe'].astype(float)
	df['mittel_luftdruck'] = df['mittel_luftdruck'].astype(float)
	df.to_sql('wettermessung', engine, index=False, if_exists='replace', schema='wetter')
except:
	print("Import der Wettermessungen fehlgeschlagen!")

try:
	print("Lösche Dateien...")
	os.remove("wetterdaten_Wettermessung.csv")
	os.remove("wetterdaten_Wetterstation.csv")
	os.remove("wetterdaten.zip")
except:
	print("Konnte Dateien nicht löschen.")

try:
	print("Erzeuge Geo-Daten!")
	with engine.connect() as con:
	    con.execute('alter table wetter.wetterstation add column location geography;')
	    con.execute('update wetter.wetterstation set location=ST_SetSRID(ST_MakePoint(Geo_Laenge, Geo_Breite), 4326);')
	    con.execute('ALTER TABLE wetter.wetterstation DROP COLUMN Geo_Laenge;')
	    con.execute('ALTER TABLE wetter.wetterstation DROP COLUMN Geo_Breite;')
except:
	print("Konnte Geo-Daten nicht erzeugen!")