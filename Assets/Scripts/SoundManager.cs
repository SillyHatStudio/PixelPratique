using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{

	public static SoundManager SPEAKER;
	

	public AudioClip[] m_MusicList;
	public AudioClip[] m_SoundList;

	public AudioClip m_NextMusic;

	private AudioSource m_SoundSystem;

	void Awake()
    {
        Debug.Log("Awake SoundManager");
		if (SPEAKER == null)
        {
			SPEAKER = this;
			DontDestroyOnLoad(SPEAKER.gameObject);
            if (!Init())
            {
                Debug.LogError("Unable to init soundManager!");
            }
        }
        else
        {
            Debug.LogError("SoundManager already exists!");
        }
    }

	// Use this for initialization
	bool Init () 
	{	
		bool initiated = false;
		m_SoundSystem = GetComponent<AudioSource>();
		m_SoundSystem.loop = true;
		initiated = true;
		return initiated;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckFadeOut();
	}

	public void PlaySound(string _soundName)
	{
		AudioClip clip = null;

		for(int i = 0; i < m_SoundList.Length; i++)
		{
			if(m_SoundList[i].name == _soundName)
			{
				clip = m_SoundList[i];
			}
		}

		if(clip != null)
		{
			m_SoundSystem.PlayOneShot(clip);
		}
	}

	public void PlayMusic(string _musicName)
	{
		AudioClip clip = null;
		
		for(int i = 0; i < m_MusicList.Length; i++)
		{
			if(m_MusicList[i].name == _musicName)
			{
				clip = m_MusicList[i];
			}
		}

		if(clip != null )
		{
			if(m_SoundSystem.clip == null)
			{
				m_SoundSystem.clip = clip;
                m_SoundSystem.volume = 0.6f;
				m_SoundSystem.Play();
			}
			else
			{
				m_NextMusic = clip;
			}
		}
		else
		{
			print(_musicName + " doesn't exist!" );
		}


	}

	void CheckFadeOut()
	{
		if(m_NextMusic != null)
		{
			if(m_NextMusic.name == m_SoundSystem.clip.name)
			{
				m_NextMusic = null;
				return;
			}

			if(m_SoundSystem.volume != 0)
			{
				m_SoundSystem.volume -= Time.deltaTime;
				if(m_SoundSystem.volume <= 0)
				{
					m_SoundSystem.clip = m_NextMusic;
					m_NextMusic = null;
					m_SoundSystem.volume = 1;
					m_SoundSystem.Play();
				}
			}
		}

	}





}
