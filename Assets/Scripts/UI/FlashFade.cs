//This script causes an image to flash into view and then fade away. It does this by adjusting the alpha value of the image

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashFade : MonoBehaviour
{
	[SerializeField]
	private Image _image;										//The image to flash
	[SerializeField]
	private Color _flashColor = new Color(1f, 0f, 0f, 0.1f);	//The color to flash the image (default is red)
	[SerializeField]
	private float _flashSpeed = 5f;								//How fast to flash the image

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Reset()
	{
		//Grab the needed reference
		_image = GetComponent<Image> ();
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	//Called by the PlayerHealth script when the player takes damage
	public void Flash()
	{
		//If the image is currently flashing, stop it
		StopCoroutine("Fade");
		//Set the image to the desired color (which causes it to appear)
		_image.color = _flashColor;
		//Start the Fade() coroutine
		StartCoroutine("Fade");
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------

	//This coroutine fades an image out over time
	private IEnumerator Fade()
	{
		//While the alpha of the image is greater than .01f (very small, we use this value instead of
		//0 because it can take a very long time for a Lerp method to reach true 0)...
		while (_image.color.a > 0.01f)
		{
			//...Reduce the alpha using a Lerp (short for linear interpolation)...
			_image.color = Color.Lerp(_image.color, Color.clear, _flashSpeed * Time.deltaTime);
			//...and then wait a frame before looping
			yield return null;
		}
		//Set the alpha of the image to 0 in case there is still a small alpha value
		_image.color = Color.clear;
	}
}
