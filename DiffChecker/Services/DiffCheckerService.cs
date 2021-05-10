using System;
using System.Collections.Generic;
using DiffChecker.Errors;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;

namespace DiffChecker.Services
{
    public class DiffCheckerService : IDiffCheckerService
    {
        private readonly IRepository repository;
        private readonly IDecodeService decodeService;

        public DiffCheckerService(
            IRepository repository,
            IDecodeService decodeService)
        {
            this.repository = repository;
            this.decodeService = decodeService;
        }

        public void SetLeft(string id, string data)
        {
            repository.SetLeft(id, data);
        }

        public void SetRight(string id, string data)
        {
            repository.SetRight(id, data);
        }

        public ServiceResponse FindDifference(string id)
        {
            var encodedLeft = repository.GetLeft(id);
            var encodedRight = repository.GetRight(id);

            if (string.IsNullOrEmpty(encodedLeft)) throw new MissingInputException($"Left content for {id} is not valid");
            if (string.IsNullOrEmpty(encodedRight)) throw new MissingInputException($"Right content for {id} is not valid");

            var left = DecodeString(encodedLeft);
            var right = DecodeString(encodedRight);

            if (left.Length != right.Length)
            {
                return new ServiceResponse
                {
                    DifferentSize = true
                };
            }

            if (left == right)
            {
                return new ServiceResponse
                {
                    Equal = true
                };
            }

            var diffPoints = FindDifferencePoints(left, right);
            return new ServiceResponse
            {
                Equal = false,
                DiffPoints = diffPoints
            };
        }

        private string DecodeString(string encodedData)
        {
            return decodeService.DecodeString(encodedData);
        }

        private IList<DiffPoint> FindDifferencePoints(string decodedLeftString, string decodedRightString)
        {
            var stringLength = decodedLeftString.Length;
            var charCounter = 0;

            var diffPoints = new List<DiffPoint>();
            DiffPoint currentDiffPoint = null;

            Func<bool> differingChars = () => decodedLeftString[charCounter] != decodedRightString[charCounter];
            Action addDiffPointAsNecessary = () => { if (currentDiffPoint != null) { diffPoints.Add(currentDiffPoint); } };
            Action processDifferingChar = () =>
            {
                if (currentDiffPoint == null)
                {
                    currentDiffPoint = new DiffPoint
                    {
                        Offset = charCounter,
                        Length = 1
                    };
                }
                else
                {
                    currentDiffPoint.Length++;
                }
            };
            Action processSameChar = () =>
            {
                addDiffPointAsNecessary();
                currentDiffPoint = null;
            };
            Action finalize = () => addDiffPointAsNecessary();

            while (charCounter < stringLength)
            {
                var action = differingChars() ? processDifferingChar : processSameChar;
                action();
                charCounter++;
            }

            finalize();

            return diffPoints;
        }
    }
}