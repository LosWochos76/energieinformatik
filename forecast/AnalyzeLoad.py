import pandas as pd
from sqlalchemy import create_engine
from Decomposer import Decomposer
from HoltWinters import HoltWinters
import matplotlib.pyplot as plt

engine = create_engine('postgresql://postgres@localhost:5432/energieinformatik')
data = pd.read_sql("SELECT timestamp, load as data FROM load where timestamp >= '2018-10-15' and timestamp < '2018-11-01' order by timestamp", engine)

#decomp = Decomposer(data, 24)
#decomp.decompose()
#decomp.plot()

#from statsmodels.tsa.seasonal import seasonal_decompose
#from matplotlib import pyplot
#data = data.set_index("timestamp")
#result = seasonal_decompose(data, model='additive', freq=24)
#result.plot()
#pyplot.show()

hw = HoltWinters(data)
hw.hw1(0.5, 0, 100)
hw.plot()