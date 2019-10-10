from pyomo.environ import *

model = ConcreteModel(name="Kraftwerksbetrieb")
model.KohleMW = Var(domain=NonNegativeReals)
model.GasMW = Var(domain=NonNegativeReals)

# declare objective
model.profit = Objective(
    expr = 10*model.KohleMW + 2*model.GasMW,
    sense = maximize)

# declare constraints
model.KohleBound = Constraint(expr = model.KohleMW <= 200)
model.GasBound = Constraint(expr = model.GasMW <= 200)
model.Demand = Constraint(expr = model.KohleMW + model.GasMW == 300)
model.Co2Bound = Constraint(expr = model.KohleMW + 0.575*model.GasMW <= 230)

# solve
SolverFactory('glpk').solve(model).write()

# display solution
print("Profit = ", model.profit())
print("KohleMW = ", model.KohleMW())
print("GasMW = ", model.GasMW())

