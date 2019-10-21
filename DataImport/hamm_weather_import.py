# Import weather data for the city of Hamm into a postgreSQL-database.
# You need to download the weather-package from OPSD first!

import pandas as pd
from sqlalchemy import create_engine

cols=["utc_timestamp", "DEA5_windspeed_10m", "DEA5_temperature", "DEA5_radiation_direct_horizontal", "DEA5_radiation_diffuse_horizontal"]
df = pd.read_csv('weather_data.csv', sep=',', usecols=cols)
df['utc_timestamp'] = pd.to_datetime(df['utc_timestamp'])
df['DEA5_windspeed_10m'] = df['DEA5_windspeed_10m'].astype(float)
df['DEA5_temperature'] = df['DEA5_temperature'].astype(float)
df['DEA5_radiation_direct_horizontal'] = df['DEA5_radiation_direct_horizontal'].astype(float)
df['DEA5_radiation_diffuse_horizontal'] = df['DEA5_radiation_diffuse_horizontal'].astype(float)

df = df.rename(columns={
  "utc_timestamp":"timestamp", 
  "DEA5_windspeed_10m":"windspeed_10m", 
  "DEA5_temperature":"temperature", 
  "DEA5_radiation_direct_horizontal":"radiation_direct_horizontal", 
  "DEA5_radiation_diffuse_horizontal":"radiation_diffuse_horizontal"})

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
df.to_sql('weather', engine, index=False, if_exists='replace')