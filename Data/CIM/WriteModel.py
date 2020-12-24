import uuid
from CIM14.IEC61970.Core import ConnectivityNode
from CIM14.IEC61970.Core import Terminal
from CIM14.IEC61970.Wires import SynchronousMachine
from CIM14.IEC61970.Wires import Breaker
from CIM14.IEC61970.LoadModel import ConformLoad
from CIM14.IEC61970.Wires import EnergyConsumer
from PyCIM import cimwrite

g1 = SynchronousMachine(name="Generator", UUID=str(uuid.uuid4()))
t1 = Terminal(name="Terminal 1", UUID=str(uuid.uuid4()))
t2 = Terminal(name="Terminal 2", UUID=str(uuid.uuid4()))
t3 = Terminal(name="Terminal 3", UUID=str(uuid.uuid4()))
t4 = Terminal(name="Terminal 4", UUID=str(uuid.uuid4()))
c1 = ConnectivityNode(name='Node 1', UUID=str(uuid.uuid4()))
c2 = ConnectivityNode(name='Node 2', UUID=str(uuid.uuid4()))
b1 = Breaker(name="Breaker", UUID=str(uuid.uuid4()))
l1 = EnergyConsumer(name="Consumer", UUID=str(uuid.uuid4()))

g1.addTerminals(t1)
c1.addTerminals(t1, t2)
b1.addTerminals(t2, t3)
c2.addTerminals(t3, t4)
l1.addTerminals(t4)

d = {}
for elem in [g1, t1, t2, t3, t4, c1, c2, b1, l1]:
    d[elem.UUID] = elem

cimwrite(d, "simple_model.xml")