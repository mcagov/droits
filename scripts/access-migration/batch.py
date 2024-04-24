import pandas as pd

# Read the original CSV file
file_path = './data/Access-Closed-Droits.csv'
df = pd.read_csv(file_path, encoding='ISO-8859-1')

# Split the DataFrame into batches
batch_size = 50
num_batches = len(df) // batch_size + (len(df) % batch_size > 0)

# Write headers to the first batch file
first_batch_df = df.iloc[:batch_size]
first_batch_file_path = './data/batch_1.csv'
first_batch_df.to_csv(first_batch_file_path, index=False)

print('Batch 1 saved to {}'.format(first_batch_file_path))

# Write the remaining batches
for i in range(1, num_batches):
    start_idx = i * batch_size
    end_idx = start_idx + batch_size
    batch_df = df.iloc[start_idx:end_idx]

    # Create a new CSV file for each batch
    batch_file_path = './data/batch_{}.csv'.format(i + 1)
    batch_df.to_csv(batch_file_path, index=False)

    print('Batch {} saved to {}'.format(i + 1, batch_file_path))
