using UnityEngine;

public static class SmoothDampUtility
{
	public static Quaternion QuaternionSmoothDamp(Quaternion current, Quaternion target, ref Quaternion smoothVelocity, float smoothTime)
	{
		if (Time.deltaTime < Mathf.Epsilon)
			return current;

		float Dot = Quaternion.Dot(current, target);
		float Multi = Dot > 0f ? 1f : -1f;
		target.x *= Multi;
		target.y *= Multi;
		target.z *= Multi;
		target.w *= Multi;

		Vector4 Result = new Vector4(
			Mathf.SmoothDamp(current.x, target.x, ref smoothVelocity.x, smoothTime),
			Mathf.SmoothDamp(current.y, target.y, ref smoothVelocity.y, smoothTime),
			Mathf.SmoothDamp(current.z, target.z, ref smoothVelocity.z, smoothTime),
			Mathf.SmoothDamp(current.w, target.w, ref smoothVelocity.w, smoothTime)
		).normalized;


		Vector4 derivError = Vector4.Project(new Vector4(smoothVelocity.x, smoothVelocity.y, smoothVelocity.z, smoothVelocity.w), Result);
		smoothVelocity.x -= derivError.x;
		smoothVelocity.y -= derivError.y;
		smoothVelocity.z -= derivError.z;
		smoothVelocity.w -= derivError.w;

		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}

    public static Color ColorSmoothDamp(Color current, Color target, ref Color smoothVelocity, float smoothTime)
    {
        return new Color(
            Mathf.SmoothDamp(current.r, target.r, ref smoothVelocity.r, smoothTime),
            Mathf.SmoothDamp(current.g, target.g, ref smoothVelocity.g, smoothTime),
            Mathf.SmoothDamp(current.b, target.b, ref smoothVelocity.b, smoothTime),
            Mathf.SmoothDamp(current.a, target.a, ref smoothVelocity.a, smoothTime)
		);
    }
}