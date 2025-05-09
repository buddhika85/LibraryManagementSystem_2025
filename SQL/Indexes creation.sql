

use LMS;

---- create indexes for fast filering 

CREATE INDEX IX_Borrowals_Email ON AspNetUsers(Email);
CREATE INDEX IX_Borrowals_DueDate ON Borrowals(DueDate);
