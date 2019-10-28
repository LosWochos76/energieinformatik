from pyomo.environ import *

model = ConcreteModel()
model.KohleMW = Var(domain=NonNegativeReals, bounds=(0,200))
model.GasMW = Var(domain=NonNegativeReals, bounds=(0,200))

# declare objective
model.profit = Objective(
    expr = 10 * model.KohleMW + 2 * model.GasMW,
    sense = maximize)

# declare constraints
model.Demand = Constraint(expr = model.KohleMW + model.GasMW == 300)
model.Co2Bound = Constraint(expr = model.KohleMW + 0.575*model.GasMW <= 230)

# solve
opt = SolverFactory('glpk')
result = opt.solve(model)
result.write()

# display solution
print("KohleMW = ", model.KohleMW.value)
print("GasMW = ", model.GasMW.value)