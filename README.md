## Simple example 

```markdown
connection = new EAC_Framework.eac_sqlConnector("Server", "dataBase", "Table", "User", "Password");
gridView = new EAC_Framework.eac_dataGridView(ref dataGridView1); 
gridView.fillGridFromSqlSelect("YOUR QUERY HERE",ref connection);
```
note: dataGridView1 is your object in C# 
# Methods  

## setAutoInsert(bool)
Used for enable/disable auto insert on dataGridView.
## setAutoUpdate(bool)
Used for enable/disable auto update when you edit a cell in dataGridView.

### Support or Contact
cardenaz_1000@hotmail.com
