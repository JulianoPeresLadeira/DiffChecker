using System;
using System.Collections.Generic;
using DiffChecker.Domain.Error;
using DiffChecker.Domain.Model;
using DiffChecker.Domain.Services;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;

namespace DiffChecker.Services
{
    public class DiffCheckerService : IDiffCheckerService
    {
        private readonly IRepository _repository;
        private readonly IDecodeService _decodeService;

        public DiffCheckerService(
            IRepository repository,
            IDecodeService decodeService)
        {
            _repository = repository;
            _decodeService = decodeService;
        }

        public DiffData SetLeft(string id, string data)
        {
            return _repository.SetLeft(id, data);
        }

        public DiffData SetRight(string id, string data)
        {
            return _repository.SetRight(id, data);
        }

        public ComparisonResponse FindDifference(string id)
        {
            var encodedLeft = _repository.GetLeft(id)?.Data;
            var encodedRight = _repository.GetRight(id)?.Data;

            if (string.IsNullOrEmpty(encodedLeft)) throw new MissingInputException($"Left content for {id} is not valid");
            if (string.IsNullOrEmpty(encodedRight)) throw new MissingInputException($"Right content for {id} is not valid");

            var left = DecodeString(encodedLeft);
            var right = DecodeString(encodedRight);

            if (left.Length != right.Length)
            {
                return new ComparisonResponse
                {
                    DifferentSize = true
                };
            }

            if (left == right)
            {
                return new ComparisonResponse
                {
                    Equal = true
                };
            }

            var diffPoints = FindDifferencePoints(left, right);
            return new ComparisonResponse
            {
                Equal = false,
                DiffPoints = diffPoints
            };
        }

        private string DecodeString(string encodedData)
        {
            return _decodeService.DecodeString(encodedData);
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