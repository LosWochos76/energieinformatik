#!/usr/bin/python3
# import the list of renewable powerplants in germany into a postgres database
# You need to download the csv-file manually from https://data.open-power-system-data.org/renewable_power_plants/

import pandas as pd
from sqlalchemy import create_engine

df = pd.read_csv('renewable_power_plants_DE.csv', sep=',')
df['commissioning_date'] = pd.to_datetime(df['commissioning_date'])
df['decommissioning_date'] = pd.to_datetime(df['decommissioning_date'])

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
df.to_sql('renewable_power_plants', engine, index=False, if_exists='replace')