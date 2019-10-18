import pandas as pd 
import matplotlib.pyplot as plt
from pandas.plotting import autocorrelation_plot

class HoltWinters:
    def __init__(self, data):
        self.data = data
    
    def hw1(self, alpha, l0, h):
        count = len(self.data.index)
        self.data.at[0, "hw1"] = alpha * self.data.at[0, "data"] + (1-alpha) * l0
        for i in range(1, count):
            self.data.at[i, "hw1"] = alpha * self.data.at[i, "data"] + (1-alpha) * self.data.at[i-1, "hw1"]
        
        time_diff = self.data.at[1, "timestamp"] - self.data.at[0, "timestamp"]
        time = self.data.at[count-1, "timestamp"]
        last_value = self.data.at[count-1, "hw1"]
        
        for i in range(0, h):
            time = time + time_diff
            self.data = self.data.append({"timestamp":time, "hw1": last_value}, ignore_index=True)
    
    def plot(self):
        self.data.plot(x="timestamp", y=["data", "hw1"])
        plt.show()