import pandas as pd 
import matplotlib.pyplot as plt
from pandas.plotting import autocorrelation_plot

class HoltWinters:
    def __init__(self, data):
        self.data = data
    
    def hw1(self, alpha, l0, h):
        count = len(self.data.index)
        l1 = alpha * self.data.at[0, "data"] + (1-alpha) * l0
        self.data.at[0, "hw"] = l1

        for i in range(1, count):
            lt = alpha * self.data.at[i, "data"] + (1-alpha) * self.data.at[i-1, "hw"]
            self.data.at[i, "hw"] = lt
        
        time_diff = self.data.at[1, "timestamp"] - self.data.at[0, "timestamp"]
        time = self.data.at[count-1, "timestamp"]
        last_value = self.data.at[count-1, "hw"]
        
        for i in range(0, h):
            time = time + time_diff
            self.data = self.data.append({"timestamp":time, "hw": last_value}, ignore_index=True)
    
    def plot(self):
        self.data.plot(x="timestamp", y=["data", "hw"])
        plt.show()

    def hw2(self, alpha, l0, beta, b0, h):
        count = len(self.data.index)

        self.data.at[0, "l"] = alpha * self.data.at[0, "data"] + (1-alpha)*(l0 + b0)
        self.data.at[0, "b"] = beta * (self.data.at[0, "l"] - l0) + (1 - beta)*b0
        self.data.at[0, "hw"] = self.data.at[0, "l"] + self.data.at[0, "b"]

        for i in range(1, count):
            lt = alpha * self.data.at[i, "data"] + (1-alpha)*(self.data.at[i-1, "l"] + self.data.at[i-1, "b"])
            bt = beta * (self.data.at[i, "l"] - self.data.at[i-1, "l"]) + (1 - beta) * self.data.at[i-1, "b"]
            self.data.at[i, "l"] = lt
            self.data.at[i, "b"] = bt
            self.data.at[i, "hw"] = lt + bt
        
        time_diff = self.data.at[1, "timestamp"] - self.data.at[0, "timestamp"]
        time = self.data.at[count-1, "timestamp"]
        last_l = self.data.at[count-1, "l"]
        last_b = self.data.at[count-1, "b"]
        
        for i in range(0, h):
            time = time + time_diff
            self.data = self.data.append({"timestamp":time, "hw": last_l + i*last_b}, ignore_index=True)