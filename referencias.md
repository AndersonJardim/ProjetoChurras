## POST Alguém pilha?
http: //localhost:7296/api/churras
{
    "date": "2023-01-20T18:00:00",
    "reason": "Comemorar a entrada do novo dev back da mitsui",
    "isTrincasPaying": true
}

## GET E aí, vai rolar?
http: //localhost:7296/api/churras

## PUT Tem aval dos sócios?
http: //localhost:7296/api/churras/{{churras-id}}/moderar
{
    //Está participando
    "GonnaHappen": false,
    "TrincaWillPay": false
}

#### GET  Churras? Quando?
http: //localhost:7296/api/person/invites

#### PUT Aceitar convite
http: //localhost:7296/api/person/invites

#### PUT Rejeitar  convite
http: //localhost:7296/api/person/invites/3d9702aa-6f1c-437c-a3ad-bd6c1daea143/decline
{
    //é vegano
    "isVeg": false
}
