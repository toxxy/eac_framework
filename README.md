## Simple example 

```markdown
connection = new EAC_Framework.eac_sqlConnector("Server", "dataBase", "Table", "User", "Password");
gridView = new EAC_Framework.eac_dataGridView(ref dataGridView1); 
gridView.fillGridFromSqlSelect("YOUR QUERY HERE",ref connection);
```
Note: dataGridView1 is your object in C#  
Note2: Table can be empty like "" 
# Methods  

## setAutoInsert(bool)
Used for enable/disable auto insert on dataGridView.
## setAutoUpdate(bool)
Used for enable/disable auto update when you edit a cell in dataGridView.
## fillGridFromSqlSelect(string,ref eac_sqlConnector)
Used for multiple connections or multiple database collector, get propierty to auto-Fill and updates. 

### Support or Contact
cardenaz_1000@hotmail.com
