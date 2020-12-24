# load some data from postgres and plot it

import pandas as pd
from sqlalchemy import create_engine
import matplotlib.pyplot as plt

engine = create_engine('postgresql://admin:secret@localhost:5432/postgres')
sql = 'select * from wetter.wettermessung where stations_id=1078 order by Datum;'
df = pd.read_sql(sql, engine)

plt.plot(df['datum'], df['min_2m'], linestyle='-', label='Min. Temp. in 2m')
plt.plot(df['datum'], df['max_2m'], linestyle='-.', label='Max. Temp. in 2m')
plt.xlabel("Tag")
plt.ylabel("Temperatur in Grad Celsius")
plt.legend(loc="upper right")
plt.show()