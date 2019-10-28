from pyomo.environ import *
import pandas as pd
import matplotlib.pyplot as plt

model = ConcreteModel()
model.hours = RangeSet(1, 24)
model.plants = Set(initialize=['lignite','coal','gas'])

demand = pd.read_csv('demand.csv', sep=',', index_col=0)
model.demand = Param(model.hours, initialize=demand.to_dict()['demand'])

params = pd.read_csv('plants.csv', sep=',', index_col=0).to_dict()
model.production_costs = Param(model.plants, initialize=params['production_costs'])
model.startup_costs = Param(model.plants, initialize=params['startup_costs'])
model.p_min = Param(model.plants, initialize=params['p_min'])
model.p_max = Param(model.plants, initialize=params['p_max'])
model.initial_generation = Param(model.plants, initialize=params['initial_generation'])
model.max_ramp = Param(model.plants, initialize=params['max_ramp'])

model.generation = Var(model.plants, RangeSet(0, 24), domain=NonNegativeReals)
model.startup = Var(model.plants, model.hours, within=Binary)
model.has_generation = Var(model.plants, RangeSet(0, 24), within=Binary)

def fullfills_initial_generation(model, p):
    return model.generation[p, 0] == model.initial_generation[p]

model.fullfills_initial_generation = Constraint(model.plants, rule=fullfills_initial_generation)

def fullfills_demand(model, p, h):
    return model.demand[h] == sum(model.generation[p, h] for p in model.plants) 

model.fullfills_demand = Constraint(model.plants, model.hours, rule=fullfills_demand)

def fullfills_p_max(model, p, h):
    return model.generation[p, h] <= model.p_max[p] * model.has_generation[p, h]

model.fullfills_p_max = Constraint(model.plants, model.hours, rule=fullfills_p_max)

def fullfills_p_min(model, p, h):
    return model.generation[p, h] >= model.p_min[p] * model.has_generation[p, h]

model.fullfills_p_min = Constraint(model.plants, model.hours, rule=fullfills_p_min)

def fullfill_generation(model, p, h):
    return model.has_generation[p, h-1] + (1 - model.has_generation[p, h]) + model.startup[p, h] >= 1

model.fullfill_generation = Constraint(model.plants, model.hours, rule=fullfill_generation)

def fullfills_ramp(model, p, h):
    return model.generation[p, h] - model.generation[p, h-1] <= model.max_ramp[p]

model.fullfills_ramp = Constraint(model.plants, model.hours, rule=fullfills_ramp)

def objective_rule(model):
    costs = sum(model.generation[p, h] * model.production_costs[p] +
        model.startup[p, h] * model.startup_costs[p]
        for p in model.plants
        for h in model.hours)
    return costs

model.objective = Objective(rule=objective_rule, sense=minimize)

opt = SolverFactory('glpk')
result = opt.solve(model)
result.write()

result = pd.DataFrame()
for h in model.hours:
    for p in model.plants:
        result.at[h, p] = model.generation[p, h].value
        print(h," ",p," ",model.has_generation[p,h].value)

result.plot.bar(stacked=True)
plt.show()