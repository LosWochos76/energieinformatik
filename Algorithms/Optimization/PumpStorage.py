from pyomo.environ import *
import pandas as pd
import matplotlib.pyplot as plt

model = ConcreteModel()
model.hours = RangeSet(1, 24)
model.pump = Var(model.hours, domain=NonNegativeReals, bounds=(0,153))
model.generator = Var(model.hours, domain=NonNegativeReals, bounds=(0,153))
model.filling = Var(RangeSet(0, 24), domain=NonNegativeReals, bounds=(0,590))

data = pd.read_csv('prices.csv', sep=',', index_col=0)
prices = data.to_dict()['price']
model.price = Param(model.hours, initialize=prices)

model.filling_start_limit = Constraint(expr = model.filling[0] == 300)
model.filling_end_limit = Constraint(expr = model.filling[24] == 300)

def filling_limit(model, h):
    return model.filling[h] == model.pump[h] - model.generator[h] + model.filling[h-1]

model.filling_limit = Constraint(model.hours, rule=filling_limit)

def objective_rule(model):
    revenue = sum(model.generator[i] * 0.8 * 
       model.price[i] for i in model.hours)
    costs = sum(model.pump[i] * model.price[i] for i in model.hours)
    return revenue - costs

model.obj = Objective(rule=objective_rule, sense=maximize)

opt = SolverFactory('glpk')
result = opt.solve(model)
result.write()

for h in model.hours:
    data.at[h, 'filling'] = model.filling[h].value
    data.at[h, 'pump'] = model.pump[h].value
    data.at[h, 'generator'] = model.generator[h].value

plt.xlim(left=1, right=24)
plt.plot(model.filling, linestyle='-', label="Füllstand")
plt.plot(model.pump, linestyle=':', label="Pumpe")
plt.plot(model.generator, linestyle='-.', label="Generator")
plt.xlabel("Stunde")
plt.ylabel("MW / MWh")
plt.legend(loc="upper right")
plt.show()