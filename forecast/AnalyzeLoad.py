import pandas as pd
from sqlalchemy import create_engine
from Decomposer import Decomposer

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
data = pd.read_sql("SELECT timestamp, load as data FROM load where timestamp >= '2018-10-01' and timestamp < '2018-11-01' order by timestamp", engine)
decomp = Decomposer(data, 24)
decomp.decompose()
decomp.plot()