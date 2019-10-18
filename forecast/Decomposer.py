import pandas as pd 
import matplotlib.pyplot as plt
from pandas.plotting import autocorrelation_plot

class Decomposer:
    def __init__(self, data, frequency, method="additive"):
        self.data = data
        self.method = method
        self.frequency = frequency

    def __seasonal_value(self, hour):
        current = hour % 24 + 25
        sum = 0
        count = 0
        while current < len(self.data.index) - self.frequency - 1:
            sum += self.data.at[current, "detrended"]
            current += self.frequency
            count = count + 1
        return sum / count
    
    def __compute_season(self):
        for i in range(self.frequency+1, len(self.data.index) - self.frequency-1):
            self.data.at[i, "season"] = self.__seasonal_value(i)

    def __normalize_season(self):
        if self.method == "additive":
            mean = self.data["season"].mean()
            self.data["season"] - mean
            self.data["trend"] + mean
        elif self.method == "multiplicative":
            sum = self.data["season"].sum()
            self.data["season"] + (self.frequency - sum)
            self.data["trend"] - (self.frequency - sum)

    def __compute_trend(self):
        self.data["ma"] = self.data["data"].rolling(window=self.frequency).mean(center=True)
        self.data["trend"] = self.data["ma"].rolling(window=2).mean(center=True)

        if self.method == "additive":
            self.data["detrended"] = self.data["data"] - self.data["trend"]
        elif self.method == "multiplicative":
            self.data["detrended"] = self.data["data"] / self.data["trend"]
    
    def __compute_residuals(self):
        if self.method == "additive":
            self.data["residuals"] = self.data["detrended"] - self.data["season"]
        elif self.method == "multiplicative":
            self.data["residuals"] = self.data["detrended"] / self.data["season"]

    def decompose(self):
        self.__compute_trend()
        self.__compute_season()
        self.__normalize_season()
        self.__compute_residuals()
    
    def trend(self):
        return self.data["trend"]
    
    def residuals(self):
        return self.data["residuals"]
    
    def season(self):
        return self.data["season"]
    
    def plot(self):
        fig = plt.figure()
        ax1 = fig.add_subplot(411)
        self.data.plot(x="timestamp", y=["data"], ax=ax1)
        ax2 = fig.add_subplot(412)
        self.data.plot(x="timestamp", y=["trend"], ax=ax2)
        ax3 = fig.add_subplot(413)
        self.data.plot(x="timestamp", y=["season"], ax=ax3)
        ax4 = fig.add_subplot(414)
        self.data.plot(x="timestamp", y=["residuals"], ax=ax4)
        plt.show()
    
    def plot_residuals_acf(self):
        autocorrelation_plot(self.data["residuals"][self.frequency+1:-self.frequency-1])
        plt.show()
    
    def plot_residuals_histogram(self):
        plt.hist(self.data["residuals"], bins=50)
        plt.show()