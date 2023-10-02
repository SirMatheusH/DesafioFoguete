# Desafio - Foguete

Esse projeto foi muito divertido de se fazer, já fazia mais de dois anos a última vez que toquei no Unity, e na época eu não fiz muita coisa exceto um clone de Snake em 3D (que tinha uma performance horrível), mas isso não vem ao caso agora.

Vou listar a seguir os parâmetros dos níveis de dificuldade e como eu fiz pra implementar cada um, e nos setores relevantes, terá referência ao script onde fiz a implementação.

# *1 - Foguete Simples*
  1. Foguete utilizando Rigidbody: Ambos os estágios do foguete utilizam `RigidBody`s com `MeshCollider`s utilizando as meshes providenciadas.
  2. Aplicar força no foguete utilizando RigidBody.AddForce(): Ambos estágios são acelerados utilizando `AddForce()` com modo de força `ForceMode.Force`, que leva em consideração a massa de ambos (`Assets/Scripts/Stage/Miscellaneous.cs`).
  3. Terminar o combustível depois de 5 segundos. (Parar de aplicar a força): Devido aos requerimentos das regras que vieram nos próximos níveis, essa mecânica foi mudada para um nível de combustível que é utilizado conforme você aciona o botão de acelerar.
  4. Detectar altura máxima. if(rigidbody.velocity.y < 0: Implementado de maneira diferente mas funcionalmente a mesma coisa, checo `rigidbody.transform.position.y > alturaMaximaAlcançada` (`Assets/Scripts/NoseStage/NoseController.cs:149`).
  5. Suavizar a descida do foguete. (rigidbody.drag): Implementado como especificado, dou um coeficiente de arrasto grande, e uma massa pequena para o paraquedas ao abrir, e devido ao 'nariz' ter uma massa maior do que o paraquedas, ele aponta pra baixo durante a descida. (`Assets\Scripts\NoseStage\NoseController.cs:131`).
  6. Separação do primeiro compartimento utilizando lógica de programação: Feito como especificado, com a adição de partículas durante a desacoplagem para simular uma separação 'explosiva', assim como aumento no arrasto do estágio separado (devido ao mesmo se tornar menos aerodinâmico).
  7. Descida controlada do foguete, utilizando o paraquedas via programação: Implementação desse item e do item 6 se encontra na mesma função do script do item 5.

# *2 - Foguete médio*
  (Minha parte favorita do desafio)
  1. Uso de partículas para representar o escape do propulsor: Tomei uma "liberdade artística" aqui, utilizando partículas mais coloridas do que as cores ~~chatas~~ normais de propulsores de foguete, e a mesma implementação (mas com efeitos diferentes) foi feita tanto no escape quanto no estágio do nariz do foguete (`Assets\Scripts\ParticleController.cs`).
  2. Aplicar um skybox no cenário (céu): Outra liberdade artística, na implementação desse requisito, utilizei um cenário de estrelas, como se o foguete estivesse sendo lançado de um planeta alienígena ~~com atmosfera~~. (Asset cortesia de [PULSAR BYTES](https://assetstore.unity.com/packages/2d/textures-materials/sky/starfield-skybox-92717)).
  3. Utilizar um terreno da unity (unity terrain) para representar o solo: Pra contribuir pra vibe de planeta alienígena, esculpi um terreno arenoso, com textura feita por [Nobiax / Yughues](https://assetstore.unity.com/packages/2d/textures-materials/floors/yughues-free-sand-materials-12964).
  4. Aplicar um som de foguete: Assim como com as partículas do item 1, criei aqui um script genérico que reutilizei no som do propulsor do estágio do nariz, o clipe sonoro foi pegado do site [mixkit.co](https://mixkit.co/free-sound-effects/rocket/) (Space rocket full power turbine) e editado com [Audacity](https://www.audacityteam.org/) para criar um loop sonoro perfeito.
  5. Lançamento de plano inclinado (lançamento balístico): Para esse requerimento eu adicionei controles de direção ao foguete (`Assets\Scripts\Stage\Miscellaneous.cs:15`), implementado como função porque inicialmente a propulsão (e consequentemente a direção) é controlada pelo estágio inferior, e depois da separação, o controle é passado pro nariz do foguete.

# *3 - Foguete Difícil*
  1. Inserir um propulsor no segundo estágio antes de abrir o paraquedas: Implementado do mesmo jeito do item 1.2, porém menos força de aceleração é aplicada, com um "tanque de combustível" menor do que o estágio desacoplado, e o efeito sonoro tem volume mais baixo.
  2. Transferir a inércia do primeiro estágio para o segundo no momento da desacoplagem (para que ele continue o movimento de maneira suave): Implementado no item 1.1, devido ao fato de ambas as partes serem `RigidBody`s, depois da separação, o estágio cai de maneira suave, e seu coeficiente de arrasto é aumentado por não ter mais um nariz pontudo cortando a "atmosfera".
  3. Aplicar uma força lateral (representando o vento): Uma direçao aleatória nos vetores x e z é criada durante a inicialização da cena, e aplicado tanto no nariz quanto no estágio inferior até uma certa altura (`Assets\Scripts\GameController.cs:36`).
  4. Reduzir o dump do objeto quando ele tocar o solo (para que ele volte a se comportar como um objeto solto): Para ser sincero, eu não entendi esse requisito, porém acredito ter implementado ele corretamente quando utilizei `RigidBody` e `MeshCollider` em ambas partes do foguete, oque faz com que eles interajam realisticamente com o mesh do terreno.

O código foi escrito inteiramente em inglês e está bem documentado (talvez até documentado *demais*), comecei a fazer ele na quinta à noite, e foi algo que gostei muito de fazer, por mais que a empresa Unity tenha feito umas decisões *questionadoras* recentemente, o produto deles é muito simples e gostoso de se usar.

Espero que gostem do projeto, dos pequenos detalhes que adicionei, das minhas soluções para cada requerimento do desafio e que me considerem para a vaga/entrevista para a vaga de Programador :)

# Controles:
    Shift Esquerdo: ativa os propulsores de ambos estágios
    WASD: controla a orientação do foguete
    Seta pra cima e pra baixo: controla a distância da câmera
    Espaço: separa os compartimentos (se abaixo de 100 metros, separar os compartimentos também aciona o paraquedas)
    Ctrl Esquerdo: Abre o paraquedas
    R: Reinicia a cena do foguete
    R na tela de menu: Muda os vetores de rotação da câmera
    L: Liga e desliga a luz direcional da cena, amplificando o efeito de "planeta alienígena" (e também porque não tem um sol no skybox acima, então daonde está vindo essa luz?) 

Link para download de uma build executável do projeto: https://drive.google.com/file/d/13ZHYvFIYHDh8SF8Ps0NTLOfveXbnDZSh
