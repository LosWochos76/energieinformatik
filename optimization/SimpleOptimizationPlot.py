import numpy as np
import matplotlib.pyplot as plt

plt.xlim(left=0, right=300)
plt.ylim(bottom=0, top=400)
plt.axvline(x=200, color="green", label="Erzeugungskapazität Kohle")
plt.axhline(y=200, color="red", label="Erzeugungskapazität Gas")
plt.xlabel("Kohle")
plt.ylabel("Gas")
plt.plot([0,300],[300,0], label="Last")
plt.plot([0,230],[400,0], label="Co2-Grenze")
plt.legend(loc="upper right")
plt.show()