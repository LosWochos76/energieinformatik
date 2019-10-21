# load some data from postgres and plot it

import pandas as pd
from sqlalchemy import create_engine
import matplotlib.pyplot as plt

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
sql = 'select * from wetter.wettermessung where stations_id=1078 order by Datum;'
df = pd.read_sql(sql, engine)

df.plot(x='datum', y=['min_2m', 'max_2m'])
plt.show()