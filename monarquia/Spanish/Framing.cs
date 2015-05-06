﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class Frame
	{
		public readonly PointOfView PointOfView;
		public readonly Conjugation Conjugation;

		public Frame (PointOfView pointOfView, Conjugation conjugation)
		{
			this.PointOfView = pointOfView;
			this.Conjugation = conjugation;
		}

		static public IEnumerable<Frame> SelectAllFrames ()
		{
			List<Frame> frames = new List<Frame> ();

			var pointsOfView = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ();

			frames.AddRange (from p in pointsOfView
				from c in Enum.GetValues (typeof(Conjugation)).Cast<Conjugation> ()
				select new Frame (p, c));

			return frames;
		}

		static public IEnumerable<Frame> FramesCoveringEachConjugationForm (string infinitive)
		{
			var random = new Random ();

			List<PointOfView> pointsOfView = new List<PointOfView> ();

			if (infinitive == "haber") {

				pointsOfView.Add (new [] {
					PointOfView.ThirdPersonMasculine,
					PointOfView.ThirdPersonFeminine
				} [ random.Next (2)]);

				pointsOfView.Add (new [] {
					PointOfView.ThirdPersonPluralMasculine,
					PointOfView.ThirdPersonPluralFeminine
				} [random.Next (2)]);

			} else {

				pointsOfView.Add (PointOfView.FirstPerson);
				pointsOfView.Add (PointOfView.SecondPerson);

				pointsOfView.Add (new [] {
					PointOfView.SecondPersonFormal,
					PointOfView.ThirdPersonMasculine,
					PointOfView.ThirdPersonFeminine
				} [ random.Next (3)]);

				pointsOfView.Add (PointOfView.FirstPersonPlural);

				pointsOfView.Add (new [] {
					PointOfView.SecondPersonPluralFormal,
					PointOfView.ThirdPersonPluralMasculine,
					PointOfView.ThirdPersonPluralFeminine
				} [random.Next (3)]);
			}

			return
				from p in pointsOfView
				from c in Enum.GetValues (typeof(Conjugation)).Cast<Conjugation> ()
				select new Frame (p, c);
		}
	}

	public class RoleSelection
	{
		Frame Framing;
		Dictionary<string, ITranslateable> Roles;

		public RoleSelection(Frame framing) {
			this.Framing = framing;
			this.Roles = new Dictionary<string, ITranslateable> ();
		}
			
		public RoleSelection WithRole(string role, ITranslateable value) {
		
			var result = new RoleSelection (this.Framing);

			result.Roles = new Dictionary<string, ITranslateable> (this.Roles);

			result.Roles.Add (role, value);

			return result;
		}

		public ITranslateable GetForRole(string role) {
		
			return this.Roles [role];
		}
	}
}

