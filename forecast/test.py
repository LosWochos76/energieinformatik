import pandas as pd 
import numpy as np
from Decomposer import Decomposer

def create_data():
    data = pd.DataFrame(columns={"timestamp", "data"})
    data["timestamp"] = pd.date_range(start='2019-01-01', end='2019-02-01', freq="h")
    
    season = (np.cos(np.linspace(-np.pi, 61*np.pi, 745))+1.5)*50
    f = lambda x: x*0.5
    trend = f(np.linspace(0, 20, 745)) 
    residuals = np.random.normal(0, 10, 745)

    data["data"] = season + trend + residuals
    return data

data = create_data()
decomp = Decomposer(data, 24, method="multiplicative")
decomp.decompose()
decomp.plot_residuals_acf()