import pandas as pd
from sqlalchemy import create_engine
from Decomposer import Decomposer

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
data = pd.read_sql("SELECT timestamp, temperature as data FROM weather " +
    "where timestamp >= '2015-08-01' and timestamp < '2015-09-01' order by timestamp", engine)

decomp = Decomposer(data, 24, method="multiplicative")
decomp.decompose()
decomp.plot()
#decomp.plot_residuals_acf()