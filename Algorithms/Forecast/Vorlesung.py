import pandas as pd 
import numpy as np
import matplotlib.pyplot as plt
from statsmodels.tsa.holtwinters import ExponentialSmoothing

data = pd.read_csv('data.csv', decimal=',', sep=';')

data.plot()
model = ExponentialSmoothing(data["data"], trend='add', seasonal='add', seasonal_periods=24)
fit = model.fit()
fit.fittedvalues.plot()
fit.forecast(96).plot()
plt.show()