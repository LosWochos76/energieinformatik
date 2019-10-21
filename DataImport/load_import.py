#!/usr/bin/python3
# import load-data into a postgres-database

import pandas as pd
from sqlalchemy import create_engine

# You need to download the timeseries-package from OPSD first!
cols = ["utc_timestamp", "DE_load_actual_entsoe_transparency"]
df = pd.read_csv('time_series_60min_singleindex.csv', sep=',', usecols=cols)
df.columns = ["timestamp", "load"]
df['timestamp'] = pd.to_datetime(df['timestamp'])
df['load'] = df['load'].astype(float)
df = df.dropna()

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
df.to_sql('load', engine, index=False, if_exists='replace')