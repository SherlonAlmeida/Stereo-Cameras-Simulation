Step by Step:
    -Criar câmera left
    -Criar câmera right
    -Criar renderer + quad para visualização da câmera left
    -Criar renderer + quad para visualização da câmera right
    -Criar Script + Material + Shader + Quad para visualização de Depth da câmera left

Anotações Importantes:
    -É possível obtermos depth preciso usando raycast no Unity 3D!
    -Obs.: as distâncias medidas são baseadas no collider do objeto, então eles precisam estar precisos!
        -> Observei que um cilindro é criado com capsule collider, o que parece com uma esfera no depth map! ERRADO!
    ->Para mudar a resolução da imagem exportada:
        -Altere a resolução de "Assets/CameraInfo/RenderLeft".
        -Altere a resolução de "Assets/CameraInfo/RenderRight".
        -Altere a resolução do GameObject "DepthManager".
	-Colocar todos com resolução 3000x2000 para exportar (ou 300x200) para visualizar em Gamemode.
    -> Para exportar as imagens basta pressionar "p" em Gamemode.