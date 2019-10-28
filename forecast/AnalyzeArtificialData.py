# Create some artificial seasonal data and analyze it

import pandas as pd 
import numpy as np
import matplotlib.pyplot as plt
from HoltWinters import HoltWinters

def create_data():
    data = pd.DataFrame(columns={"timestamp", "data"})
    data["timestamp"] = pd.date_range(start='2019-01-01', end='2019-02-01', freq="h")
    
    season = (np.cos(np.linspace(-np.pi, 61*np.pi, 745))+1.5)*50
    f = lambda x: x*3
    trend = f(np.linspace(0, 20, 745)) 
    residuals = np.random.normal(0, 10, 745)

    data["data"] = season + trend + residuals
    return data

data = create_data()
#data.plot(x='timestamp', y=['data'])
#plt.show()

#hw = HoltWinters(data)
#hw.hw2(0.5, 1, 0.001, 1, 20)
#hw.plot()

#from statsmodels.tsa.holtwinters import SimpleExpSmoothing
#fit = SimpleExpSmoothing(data["data"]).fit()
#fcast = fit.forecast(20)
#fcast.plot()
#fit.fittedvalues.plot()
#plt.show()

#from statsmodels.tsa.holtwinters import Holt
#fit = Holt(data["data"]).fit(optimized=True)
#fcast = fit.forecast(20)
#fcast.plot()
#fit.fittedvalues.plot()
#plt.show()

from statsmodels.tsa.holtwinters import ExponentialSmoothing
fit = ExponentialSmoothing(data["data"], trend='add').fit()
fcast = fit.forecast(48)
fcast.plot()
fit.fittedvalues.plot()
plt.show()