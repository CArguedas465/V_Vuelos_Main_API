# V_Vuelos_Main_API
Repositorio para la construcción de un API que interactúe con la base de datos principal del proyecto V_Vuelos.

NOTAS IMPORTANTES: 

- La instancia de la base de datos local para el API debe de llamarse "V_Vuelos_Main" para evitar conflictos con código preprogramado. Una vez creada la instancia de la BD, se puede utilizar el archivo "DDL" en el repositorio de *V_Vuelos > Base de Datos > DDL* para crear las tablas y los stored procedures de manera automática. Ya que es DDL, este script NO INCLUYE INSERCIONES. 

- Cuando se cree la instancia de la base de datos en el equipo local, se debe descargar este repositorio de API, abrir la solución, borrar el archivo Model1.edmx de la carpeta MODELS, y recrear un modelo .edmx según lo que se vé en la grabación. Esto ÚNICAMENTE DEBE HACERSE cuando ya se tenga creada la base de datos.

- La información de la base de datos se encuentra codificada por medio de BASE64 encoding. Este es proveído por el API, por lo que TODOS LOS DATOS INTRODUCIDOS A LA BASE DE DATOS DEBEN PROVENIR DE MÉTODOS "POST" DEL API. 

- La información que se introduzca a la BD tiene que entrar en un orden muy específico, ya que esta utiliza llaves primarias con "identity" habilitado, por lo que el valor de estas incrementa automáticamente conforme se inserten los valores. El orden en el que se debe insertar la información (así como los datos que deben ser insertados), se encuentra en el archivo "DML" del repositorio V_Vuelos. 

Este archivo es SOLO DE REFERENCIA, y no debe ser usado para popular la base de datos, ya que realiza una encriptación de lado del SQL Server, no del API, que es lo que se necesita. 
