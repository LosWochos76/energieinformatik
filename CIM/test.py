from CIM14.IEC61970.Core import ConnectivityNode
from CIM14.IEC61970.Core import Terminal
from CIM14.IEC61970.Wires import SynchronousMachine
from CIM14.IEC61970.Wires import Breaker
from CIM14.IEC61970.LoadModel import ConformLoad
from PyCIM import cimwrite

g1 = SynchronousMachine(name="Generator")
t1 = Terminal(name="Terminal 1")
t2 = Terminal(name="Terminal 2")
t3 = Terminal(name="Terminal 3")
t4 = Terminal(name="Terminal 4")
c1 = ConnectivityNode(name='Node 1')
c2 = ConnectivityNode(name='Node 2')
b1 = Breaker(name="Breaker")
l1 = ConformLoad(name="Load")

g1.addTerminals(t1)
c1.addTerminals(t1, t2)
b1.addTerminals(t2, t3)
c2.addTerminals(t3, t4)
l1.addTerminals(t4)
