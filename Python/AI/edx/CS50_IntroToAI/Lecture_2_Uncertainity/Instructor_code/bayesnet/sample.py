from pomegranate.distributions import ConditionalCategorical

from collections import Counter

from model import model

# Rejection sampling
# Compute distribution of Appointment given that train is delayed
N = 10000
data = []
for i in range(N):
    sample = model.sample(1)[0]
    # sample == "delayed"
    if sample[2] == 1.0:
        data.append("attend" if sample[3] == 0 else "miss")
        
print(Counter(data))