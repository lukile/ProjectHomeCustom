#

## :bulb: dialogflow smart-home agent

### :construction: Introduction

Le but de ce projet est de construire un agent avec dialogflow qui a le super pouvoir de contrôler votre maison
:house_with_garden: ... intelligente :wink: </br>
Une maison virtuelle est mise en place pour chaque groupe via openhab, un framework open source de domotique (home automation). </br>

#### 1. :electric_plug: Openhab

Openhab expose les différents devices via une interface de contrôle web ou via l'application mobile</br>
[![openhab-iOS](https://img.shields.io/static/v1?label=OPENHAB&message=IOS&color=BLACK&style=for-the-badge&logo=apple "download openhab")](https://apps.apple.com/us/app/openhab/id492054521)
[![openhab-android](https://img.shields.io/static/v1?label=OPENHAB&message=Android&color=GREEN&style=for-the-badge&logo=google-play "download openhab")](https://play.google.com/store/apps/details?id=org.openhab.habdroid) 
[![openhab-web](https://img.shields.io/static/v1?label=OPENHAB&message=WEB&color=BLACK&style=for-the-badge&logo=google-chrome "download openhab")](http://62.35.68.9:8080/basicui/app?sitemap=demo)</br>
Pour brancher l'application sur l'instance openhab :black_circle: Settings </br>
:arrow_right: Remote URL :point_right: <http://62.35.68.9:8080> </br>
:arrow_right: Ignore SSL Certificates :point_right: :white_check_mark:

<p align="center">
  <img src="/docs/basic-ui-demo.png">
</p>

#### 2. Openhab Proxy API :large_orange_diamond: 

[![run in-postman](https://img.shields.io/static/v1?label=POSTMAN&message=RUN&color=ORANGE&style=for-the-badge&logo=postman "run in postman")](https://app.getpostman.com/run-collection/cd15f5845da80fe68c36)

Pour vous faciliter le développement, un proxy à été dévelopé afin de gérer une configuration (maison) par groupe et exposer un seul point d'accés pour tout le monde </br>
:point_right: [@ documentation](https://openhabproxyapi-dev-as.azurewebsites.net/index.html) </br>
:point_right: [@ API base url](https://openhabproxyapi-dev-as.azurewebsites.net/api) </br>

Le proxy vous permettra de :
+ Générer a été dévelopé la volée vos entités dialogflow
+ Récupérer votre configuration (maison, zones, chambres, devices)
+ Récupérer ou mettre à jour l'état d'un device
+ Un passe-plat (gateway) vers l'api ITEM de openhab (pour les plus fous :smiling_imp:)  

#### 3. Setup :computer:

##### 1. Créer un agent dialogflow from scratch, (skip to next step si vous avez déjà créer votre agent)</br>
   >:triangular_flag_on_post: : Vous pouvez aussi importer le built-in agent smart-home et l'éditer en ajoutant les entitées générés via le proxy (en modifiant aussi les intent + training phrases)

   + Télécharger le quickstart-agent, celui-ci contient les entités de base + un intent de demo</br>
   [![import-agent](https://img.shields.io/static/v1?label=quickstart-agent&message=download&color=BLUE&style=for-the-badge&logo=google-assistant "download quickstart-agent")](https://github.com/badreddine-dlaila/smart-home.agent/raw/master/src/quickstart-agent.zip) </br></br>
   + Importer le zip dans l'agent
     ![import-agent](/docs/import-agent.png) </br></br>
   + Récupérer la configuration des Entities via le proxy au format csv et les ajouter dans dialogflow 
     ![generate-and-import-entity-demo](/docs/generate-and-import-entity-demo.gif) </br></br>
   + Créer les intents </br>
    On peut par exemple imaginer deux intents différents:</br>
     :point_right: récupérer l'état d'un device (ex: smarthome.device.state.check ) </br> 
     :point_right: commander un device (ex: smarthome.device.command ) </br>
     :rocket: voici un exemple complet [smart-home-demo](https://github.com/badreddine-dlaila/smart-home.agent/blob/master/src/Smart-Home-Demo.zip) </br></br>
    ![create-entity-demo](/docs/create-entity-demo.gif) </br></br>

##### 2. Dev/Test dalogflow fulfillment webhook
+ Cloner le project :ghost:

+ Installer les packages NPM
  ```bash
  ~ > cd src/dialogflow-fulfillment/ 
  ~\dialogflow-project\src\dialogflow-fulfillment > npm install
  ```
+ Remplacer `<access-token>` dans `/src/index.ts` </br></br>
  ```typescript
  const openhabClient = new OpenhabClient('https://openhabproxyapi-dev-as.azurewebsites.net', '<key>');
  ```
+ créer un compte ngrok et récupérer le `Tunnel AuthToken`  [:point_right: depuis le dashboard ngrok](https://dashboard.ngrok.com/auth)</br></br>
   ![import-agent](/docs/ngrok-auth-token.png) </br></br>
+ Remplacer `<ngrok-auth-token>` dans `package.json` </br></br>
  ```json
  "tunnel": "ngrok http -authtoken <ngrok-auth-token> -host-header=localhost 8080"
  ```
+ Depuis le terminal visual studio, lancer les commandes suivantes 
  ```bash
  ~/dialogflow-project/src/dialogflow-fulfillment > npm run dev
  ```    
    ```bash
  ~/dialogflow-project/src/dialogflow-fulfillment > npm run tunnel
  ``` 
  ![vscode-npm-scripts](/docs/vscode-run-scripts.gif) </br></br>
+ Ajouter l'url générée par ngrok + /webhook dans la config fulfillment de l'agent sur dialogflow </br></br>
  ![vscode-npm-scripts](/docs/debug-demo.gif) </br></br>

:coffee: :beer: :grinning: :fire: :rocket: :airplane: :moon: :sunny: :stars: :star:

