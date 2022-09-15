# WebFinancialHelper
<p>This Project helps you keep track of your daily expenses, allowing you to Crete, View, Edit and Delete financial records. This web application has two main features: one allows you to manually add your expenses, and the other allows you to add a photo of a financial receipt.</p>
    <p>
        This project uses OCR technology to read images and convert it to plain text, storing the data extracted from the receipt in a SQL Server Database. The application itself uses ORM technology to interact with the database.
        For the OCR technology I used the Tesseract OCR package, and for the ORM technology I used EntityFramework Core.
    </p>
    <p>The application works by asking the user for an input, that could be manually typed or a receipt photograph. If user chooses to input a photo, the application will process the image and write its content in a text file. This text file will be filtered using Regulax Expressions, converted into a Json format and written in a json file. This json file will be mapped in a model class that will be used to store the data into the database.</p>
<p>This project has a register and login feature that is used to call an external Api. The Api is reponsible to Register users to the database and to validade login requests. You can check out the Api repository in this link: https://github.com/Biankowski/UserApi</P>




https://user-images.githubusercontent.com/80427809/190437974-722727eb-adb0-4007-84a1-4e9046ed940c.mp4

