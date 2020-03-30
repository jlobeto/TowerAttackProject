Aca van a estar todas las cosas que tengan que ver con tutoriales

Va a consistir de un tutorial manager (monobehaviour) el que se va a encargar de ver que cosas son las que triggerean a los TutorialGroups para q cuando pasen, las triggeree 

Para esto va a necesitar alguna lista de estructuras de codigo de las cuales van a tener los datos de cuando triggerearse
Estas estructuras se llaman TutorialGroup
Tambien sabra cual es el tutorial que se está ejecutando y prohibirle el triggereo de otros tutorialsGroup


TutorialGroup:
	Tendran los distintos pasos para completar ese grupo de tutoriales.
	tutorialGroupID:"" > para guardar localmente si se completo o no el tutorial
	Tendra una lista de strings con regex para saber que cosas tiene q haber en escena o que cosas tiene que tener el usuario para poder triggerearse
		PorEjemplo:
			sceneName:"sceneName"
			popupOpened:"class name of popup"
			coinAmount:int
			firstTimeInGame:true > checkea si es la primera vez que el usuario ingreso
	Guardará (en tutorial manager) toda la data para que no se cargue otra vez el mismo tutorial

 	Se deberan poder guardar datos
	Debera tener el indice del step actual y saber cuando se terminaron todos los steps
	Por el momento seran jsons llamados BaseTutorialStep
	BaseTutorialStep:
		contendran un regex (en este caso puede ser un array de strings) en el cual se pondran las cosas como:
			button:true 
			elementIndex=int > siestá en una lista... que elemento de ella es
			tag:"sarasa" 
			type:"blackOverlay" 
			pointingFinger:true 
			fingerPosition:left/right/up/down 
			text:"presiona aqui" 
			textColor:"Red"
			Con esto se podrá buscar el boton por ejemplo y mostrarlo delante de un overlay en negro con transparencia, para hacerlo resaltar y que el usuario presione ese boton
		esto será un scriptable object, pero sus funciones del base script estaran orientadas a leer ese array de strings para saber que es lo que tiene que hacer
