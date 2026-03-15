import pandas as pd
from langchain_experimental.agents.agent_toolkits import create_pandas_dataframe_agent
from langchain_ollama import ChatOllama

df = pd.read_csv("data/transactions.csv", encoding="latin-1")

# --- Data Cleaning ---
print("Cleaning data to remove special characters and format currency...")

# 1. Remove emojis and other non-ASCII characters to get pure text.
# This helps the language model process the data more reliably.
for col in df.select_dtypes(include=['object']).columns:
    # This encodes the string to ASCII, ignoring characters it can't represent,
    # and then decodes it back to a clean string.
    df[col] = df[col].str.encode('ascii', 'ignore').str.decode('ascii').str.strip()

# 2. Convert currency columns to a numeric type for calculations.
for col in ['Outflow', 'Inflow']:
    df[col] = df[col].replace({r'\$': '', r',': ''}, regex=True).astype(float)

print("Data cleaning complete.")
# --- End of Data Cleaning ---

print("Loaded dataframe with shape:", df.shape)
print(df.head())

llm = ChatOllama(model="llama3.1:8b", temperature=0.0)

agent = create_pandas_dataframe_agent(
    llm,
    df,
    verbose=True,
    allow_dangerous_code=True,
    max_iterations=20,
)

def chat():
    print("\nType your questions about the CSV (or 'exit' to quit)\n")
    while True:
        q = input("You: ")
        if q.lower() in ["exit", "quit", "q"]:
            break
        try:
            answer = agent.run(q)
            print("\nAssistant:", answer, "\n")
        except Exception as e:
            print("Error:", e)

if __name__ == "__main__":
    chat()
