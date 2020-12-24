# Create some artificial seasonal data and analyze it

import pandas as pd 
import numpy as np
import matplotlib.pyplot as plt
from HoltWinters import HoltWinters

# Run the Script 'CreateArtificialData.py' first:
data = pd.read_csv('data.csv')

# Abbildung 061:
#data.plot(x='timestamp', y=['data'])
#plt.show()

# Abbildung 55:
#data["ma"] = data["data"].rolling(window=24, center=True).mean()
#data.plot(x="timestamp", y=["data", "ma"])
#plt.show()

# Abbildung 054:
#from statsmodels.tsa.seasonal import seasonal_decompose
#from matplotlib import pyplot
#data = data.set_index("timestamp")
#result = seasonal_decompose(data, model='additive', freq=24)
#result.plot()
#pyplot.show()

# Abbildung :
#from statsmodels.tsa.holtwinters import SimpleExpSmoothing
#fit = SimpleExpSmoothing(data["data"]).fit()
#fcast = fit.forecast(20)
#fcast.plot()
#fit.fittedvalues.plot()
#plt.show()

#hw = HoltWinters(data)
#hw.hw2(0.5, 1, 0.001, 1, 20)
#hw.plot()

#from statsmodels.tsa.holtwinters import Holt
#fit = Holt(data["data"]).fit(optimized=True)
#fcast = fit.forecast(20)
#fcast.plot()
#fit.fittedvalues.plot()
#plt.show()

# Abbildung 056:
#from statsmodels.tsa.holtwinters import ExponentialSmoothing
#fit = ExponentialSmoothing(data["data"]).fit()
#fcast = fit.forecast(48)
#fcast.plot(linestyle=':')
#fit.fittedvalues.plot()
#plt.show()

# Abbildung 057:
#from statsmodels.tsa.holtwinters import ExponentialSmoothing
#fit = ExponentialSmoothing(data["data"], trend='add').fit()
#fcast = fit.forecast(48)
#fcast.plot(linestyle=':')
#fit.fittedvalues.plot()
#plt.show()

# Abbildung 058:
from statsmodels.tsa.holtwinters import ExponentialSmoothing
fit = ExponentialSmoothing(data["data"], trend='add', seasonal_periods=24, seasonal='add').fit()
fcast = fit.forecast(48)
fcast.plot(linestyle=':')
fit.fittedvalues.plot()
plt.show()