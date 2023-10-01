using UnityEngine;

/**
 * Controlador de efeito sonoro genérico, ao anexar esse script e um AudioSource à um GameObject, esse controller pode começar e parar qualquer efeito sonoro.
 *
 * Utilizado no estagio e na parte superior do foguete.
 */
public class AudioController : MonoBehaviour
{
    
    private AudioSource _audioSource;
    public bool isPlaying;
    
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>(); // O GameObject que esse script está anexado deve ter um componente AudioSource adicionado.
        isPlaying = false;
    }

    public void PlayRocketBoosterSfx()
    {
        _audioSource.Play();
        isPlaying = true;
    }

    public void PauseRocketBoosterSfx()
    {
        _audioSource.Pause(); // Pause ao invés de Stop pra não fazer o audio clip recomeçar (não é SUPER necessário nesse caso devido ao fato dos clipes utilizados ter um som uniforme durante toda
        // a duração mas também não custa nada) 
        isPlaying = false;
    }
}
