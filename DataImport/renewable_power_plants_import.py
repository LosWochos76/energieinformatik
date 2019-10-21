#!/usr/bin/python3
# import the list of renewable powerplants in germany into a postgres database
# You need to download the csv-file manually from https://data.open-power-system-data.org/renewable_power_plants/

import pandas as pd
from pandas.io import sql
from sqlalchemy import create_engine

cols = ['commissioning_date', 'decommissioning_date', 'energy_source_level_1', 'energy_source_level_2', 
    'energy_source_level_3', 'technology', 'electrical_capacity', 'voltage_level', 'tso', 'dso', 'dso_id', 
    'eeg_id', 'federal_state', 'postcode', 'municipality_code', 'municipality', 'address', 'lat', 'lon',
    'data_source', 'comment']

df = pd.read_csv('renewable_power_plants_DE.csv', sep=',', usecols=cols)
df['commissioning_date'] = pd.to_datetime(df['commissioning_date'])
df['decommissioning_date'] = pd.to_datetime(df['decommissioning_date'])
df['electrical_capacity'] = df['electrical_capacity'].astype(float)
df['lat'] = df['lat'].astype(float)
df['lon'] = df['lon'].astype(float)

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
df.to_sql('renewable_power_plants', engine, index=False, if_exists='replace')

with engine.connect() as con:
    con.execute('alter table renewable_power_plants add column location geography;')
    con.execute('update renewable_power_plants set location=ST_SetSRID(ST_MakePoint(lon, lat), 4326);')
    con.execute('ALTER TABLE renewable_power_plants DROP COLUMN lat;')
    con.execute('ALTER TABLE renewable_power_plants DROP COLUMN lon;')
