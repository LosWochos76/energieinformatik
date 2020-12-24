import pandas as pd
import matplotlib.pyplot as plt

prices = {
  1:48.7,2:46.95,3:47.1,4:46.33,5:47.08,6:47.08,
  7:57.08,8:60.03,9:69.94,10:68.17,11:68.66,12:68.66,
  13:67.06,14:67.78,15:68.7,16:68.77,17:67.54,18:69.45,
  19:68.09,20:65.91,21:56.1,22:49.81,23:50.56,24:48.39
}

pd.DataFrame.from_dict(prices, orient='index', columns=['price']).plot()
plt.show()