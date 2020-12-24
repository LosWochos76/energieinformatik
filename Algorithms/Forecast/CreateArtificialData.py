# Create some artificial seasonal data

import pandas as pd 
import numpy as np

data = pd.DataFrame(columns={"timestamp", "data"})
data["timestamp"] = pd.date_range(start='2020-11-01', end='2020-11-30 23:00:00', freq="h")
rows = data.shape[0]

season = (np.cos(np.linspace(-np.pi, 61*np.pi, rows))+1.5)*50
f = lambda x: x*3
trend = f(np.linspace(0, 20, rows)) 
residuals = np.random.normal(0, 10, rows)

data["data"] = season + trend + residuals
data.to_csv('data.csv', index=False, decimal=',', sep=';')