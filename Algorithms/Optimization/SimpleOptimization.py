from pyomo.environ import *

model = ConcreteModel()
model.KohleMWh = Var(domain=NonNegativeReals, bounds=(0,200))
model.GasMWh = Var(domain=NonNegativeReals, bounds=(0,200))

# declare objective
model.profit = Objective(
    expr = 10 * model.KohleMWh + 2 * model.GasMWh,
    sense = maximize)

# declare constraints
model.Demand = Constraint(expr = model.KohleMWh + model.GasMWh == 300)
model.Co2Bound = Constraint(expr = model.KohleMWh + 0.575*model.GasMWh <= 230)

# solve
opt = SolverFactory('glpk')
result = opt.solve(model)
result.write()

# display solution
print("KohleMWh = ", model.KohleMWh.value)
print("GasMWh = ", model.GasMWh.value)