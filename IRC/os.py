import os
import platform

# Because I don't trust C#'s OS detection

f = open('os.tmp','w')

system = platform.system()

f.write(system)

f.close()
