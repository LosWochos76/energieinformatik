#!/usr/bin/python3
# Import fussball-data from uni bayreuth into a postgres-database

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

engine = create_engine('postgresql://admin:secret@localhost:5432/postgres')
#engine = create_engine('postgresql://hshl:hshl@localhost:5432/hshl')

try:
	print("Lösche alte Daten in der Datenbank...")
	with engine.connect() as con:
		con.execute('drop table bundesliga.liga;')
		con.execute('drop table bundesliga.spiel;')
		con.execute('drop table bundesliga.spieler;')
		con.execute('drop table bundesliga.verein;')
except:
	print("Einige Tabellen konnten nicht gelöscht werden!");

today = date.today()
d = today.strftime("%Y-%m-%d")
zipurl = 'https://dbup2date.uni-bayreuth.de/downloads/bundesliga/{}_bundesliga_CSV.zip'.format(d)

try:
	print("Lade Zip-Datei herunter...")
	ctx = ssl.create_default_context()
	ctx.check_hostname = False
	ctx.verify_mode = ssl.CERT_NONE
	with urllib.request.urlopen(zipurl, context=ctx) as u, open("bundesliga.zip", 'wb') as f:
	    f.write(u.read())
except:
	printf("Datei konnte nicht heruntergeladen werden!");

try:
	print("Entpacke Zip-Archiv...")
	with zipfile.ZipFile("bundesliga.zip", 'r') as zip_ref:
	    zip_ref.extractall(".")
except:
	print("Datei konnte nicht entpackt werden!")

try:
	print("Importiere Ligen in die Datenbank...")
	cols = ["Liga_Nr", "Verband", "Erstaustragung", "Meister", "Rekordspieler", "Spiele_Rekordspieler"]
	df = pd.read_csv("bundesliga_Liga.csv", sep=";", encoding = "ISO-8859-1", decimal=",", usecols=cols)
	df.columns = ['liga_nr','verband','erstaustragung','meister','rekordspieler','spiele_rekordspieler']
	for tag in ['liga_nr', 'meister', 'spiele_rekordspieler']:
		makeInt(df, tag)
	df['erstaustragung'] = pd.to_datetime(df['erstaustragung'])
	df.to_sql('liga', engine, index=False, if_exists='replace', schema='bundesliga')
except:
	print("Import der Ligen fehlgeschlagen!")

try:
	print("Importiere Spiele in die Datenbank...")
	cols = ["Spiel_ID", "Spieltag", "Datum", "Uhrzeit", "Heim", "Gast", "Tore_Heim", "Tore_Gast"]
	df = pd.read_csv("bundesliga_Spiel.csv", sep=";", encoding = "ISO-8859-1", decimal=",", usecols=cols)
	df.columns = ['spiel_id','spieltag','datum','uhrzeit','heim','gast','tore_heim','tore_gast']
	for tag in ['spiel_id', 'spieltag', 'heim', 'gast', 'tore_heim', 'tore_gast']:
		makeInt(df, tag)
	df['datum'] = pd.to_datetime(df['datum'] + " " + df['uhrzeit'])
	del df['uhrzeit']
	df.to_sql('spiel', engine, index=False, if_exists='replace', schema='bundesliga')
except:
	print("Import der Spiele fehlgeschlagen!")

try:
	print("Importiere Spieler in die Datenbank...")
	df = pd.read_csv("bundesliga_Spieler.csv", sep=";",
		encoding = "ISO-8859-1", decimal=",", 
		usecols={"Spieler_ID", "Vereins_ID", "Trikot_Nr", "Spieler_Name", "Land", "Spiele", "Tore", "Vorlagen"})
	df.columns = ['spieler_id','vereins_id','trikot_nr','spieler_name','land','spiele','tore','vorlagen']
	df['spieler_id'] = df['spieler_id'].astype(int)
	df['vereins_id'] = df['vereins_id'].astype(int)
	df['trikot_nr'] = df['trikot_nr'].astype(pd.Int32Dtype())
	df['spiele'] = df['spiele'].astype(int)
	df['tore'] = df['tore'].astype(int)
	df['vorlagen'] = df['vorlagen'].astype(int)
	df.to_sql('spieler', engine, index=False, if_exists='replace', schema='bundesliga')
except:
	print("Import der Spieler fehlgeschlagen!")

try:
	print("Importiere Vereine in die Datenbank...")
	df = pd.read_csv("bundesliga_Verein.csv", sep=";",
		encoding = "ISO-8859-1", decimal=",", usecols={"V_ID", "Name", "Liga"})
	df.columns = ['v_id','name','liga']
	df['v_id'] = df['v_id'].astype(int)
	df['liga'] = df['liga'].astype(int)
	df.to_sql('verein', engine, index=False, if_exists='replace', schema='bundesliga')
except:
	print("Import der Vereine fehlgeschlagen!")

try:
	print("Lösche CSV-Dateien...")
	os.remove("bundesliga_Liga.csv")
	os.remove("bundesliga_Spiel.csv")
	os.remove("bundesliga_Spieler.csv")
	os.remove("bundesliga_Verein.csv")
	os.remove("bundesliga.zip")
except:
	print("Einige heruntergeladene Dateien konnten nicht gelöscht werden!")