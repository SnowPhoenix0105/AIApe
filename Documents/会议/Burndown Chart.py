import json
import matplotlib.pyplot as plt
from datetime import datetime, timedelta
from collections import defaultdict
import os

# get all issues
project_id = '38467'
gitlab_private_token = 'u-KfD3ugzhwNjXgG5MwV'

get_issue_cmd = 'curl --header "PRIVATE-TOKEN: ' + gitlab_private_token + '" "https://gitlab.buaaoo.top/api/v4/projects/' \
                + project_id + '/issues?per_page=100" > all_issues.json'

os.system(get_issue_cmd)

# alpha phase start date and end date
start_date = '2021-04-21'
end_date = '2021-05-21'

# get all dates between start date and end date
dates = []
dt = datetime.strptime(start_date, "%Y-%m-%d")
date = end_date[:]
while date <= end_date:
    dates.append(date)
    dt = dt + timedelta(1)
    date = dt.strftime("%Y-%m-%d")

# count issue day by day
daily_issues_cnt = defaultdict(int)
issues_cnt = []
today = datetime.strftime(datetime.now() - timedelta(0), '%Y-%m-%d')

with open('all_issues.json', encoding='utf-8') as json_file:
    data = json.load(json_file)
for i in range(len(data)):
    daily_issues_cnt[data[i]['created_at'][0: 10]] += 1
    closed_at = data[i]['closed_at']
    if closed_at is not None:
        daily_issues_cnt[closed_at[0: 10]] -= 1

issues_cnt.append(daily_issues_cnt[start_date])
for i, key in enumerate(sorted(daily_issues_cnt.keys())):
    if i != 0:
        issues_cnt.append(issues_cnt[i-1] + daily_issues_cnt[key])
        if key == today:
            break

# plot burn down chart
expected_x = [0, len(dates) - 1]
expected_y = [daily_issues_cnt[start_date], 0]

actual_x = range(0, len(issues_cnt))
actual_y = issues_cnt

fig, ax = plt.subplots(figsize=(15, 10))
expected = plt.plot(expected_x, expected_y, color='green', label='expected')
actual = plt.plot(actual_x, actual_y, color='blue', label='actual')
plt.xticks(range(0, len(dates)), dates, rotation=45)
plt.legend()
plt.xlabel('Date')
plt.ylabel('Issues Count')
plt.savefig('burndown_chart.png')
