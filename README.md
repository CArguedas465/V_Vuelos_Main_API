# V_Vuelos_Main_API
Repositorio para la construcción de un API que interactúe con la base de datos principal del proyecto V_Vuelos.


NOTAS IMPORTANTES: 

- La información de la base de datos se encuentra codificada por medio de BASE64 encoding. Este es proveído por el API, por lo que TODOS LOS DATOS INTRODUCIDOS A LA BASE DE DATOS DEBEN PROVENIR DE MÉTODOS "POST" DEL API. 

- La información que se introduzca a la BD tiene que entrar en un orden muy específico, ya que esta utilizada llaves primarias con "identity" habilitado, por lo que el valor de estas incrementa automáticamente conforme se inserten los valores. El orden en el que se debe insertar la información de las tablas es el siguiente: 

1. TipoTarjeta
  1.1. POST que introduzca valor 'Visa'
  1.2. POST que introduzca valor 'Mastercard'
  1.3. POST que introduzca valor 'American Express'
  1.4. POST que introduzca valor 'Discover'
2. Operacion
3. Rol
4. Pregunta
5. TipoTransaccion
6. EstadoPuerta
7. EstadoVuelo
8. FormaPago
9. Usuario

Las tablas anteriores NO SON TODAS LAS DE LA BASE DE DATOS, pero comprenden aquella información que es tipo "diccionario" y proveerá a las otras tablas con datos foráneos. 
